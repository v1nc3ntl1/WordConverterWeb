(function () {
    "use strict";
    angular
        .module("productManagement")
        .controller("ProductListCtrl",
            ["wordConverterResource",
                     ProductListCtrl]);

    function ProductListCtrl(wordConverterResource) {
        var vm = this;

        wordConverterResource.query({ number: 2323432.34 },
            function(data) {
                vm.modelData = data;
            },
            function(response) {
                vm.message = response.statusText + "\r\n";
            });
    }
}());
