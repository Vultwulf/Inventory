function ProductsViewModel() {
    var self = this;

    // The list of all products
    self.productList = ko.observableArray([]);

    // The currently selected product
    self.selectedProduct = ko.observable({});

    // Full Name Form Input
    self.fullName = ko.observable();

    // Credit Card Form Input
    self.creditCardNumber = ko.observable();

    // Purchase Order Error message
    self.purchaseError = ko.observable();

    // Get the initial list of all products
    self.getProducts = function () {
        jQuery.ajax({
            type: "GET",
            dataType: "json",
            url: "/api/product/",
            success: function (data) {
                self.productList(data);
            }
        });
    }

    // Submit a purchase
    self.purchase = function () {

        jQuery('.loader').show();

        jQuery.ajax({
            type: "PUT",
            dataType: "json",
            url: "/api/product/" + self.selectedProduct().id,
            data: {
                productId: self.selectedProduct().id,
                fullName: self.fullName(),
                creditCardNumber: self.creditCardNumber
            },
            success: function (data) {
                // The purchase was successful
                jQuery('.loader').hide();
                $('#orderModal').modal('hide');
                $('#successModal').modal('show');

                // Reset model and update products
                self.fullName("");
                self.creditCardNumber("");
                self.getProducts();
            },
            statusCode: {
                400: function (message) {
                    jQuery('.loader').hide();
                    if (message.statusText === "NoInventory") {
                        // There is no inventory
                        self.purchaseError(message.responseText);
                        self.fullName("");
                        self.creditCardNumber("");
                        $('#orderModal').modal('hide');
                        $('#errorModal').modal('show');
                    } else if (message.statusText === "InvalidCreditCardNumber") {
                        // An invalid credit card number was given
                        self.purchaseError(message.responseText);
                        $('#errorModal').modal('show');
                    } else if (message.statusText === "FullNameRequired") {
                        // An invalid credit card number was given
                        self.purchaseError(message.responseText);
                        $('#errorModal').modal('show');
                    } else if (message.statusText === "PaymentGatewayFailed") {
                        // The payment gateway failed
                        self.purchaseError(message.responseText);
                        $('#errorModal').modal('show');
                    }                
                }
            }
        });
    }

    // Get the initial list of all products
    self.selectProduct = function (product) {
        self.selectedProduct(product);
    }

    // Get the latest products on load
    self.getProducts();

    // Function to format the currency
    self.formatCurrency = function (amount) {
        if (!amount) {
            return "";
        }
        amount += '';
        x = amount.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return "$" + x1 + x2;
    }
}

viewModel = ko.applyBindings(new ProductsViewModel()); 