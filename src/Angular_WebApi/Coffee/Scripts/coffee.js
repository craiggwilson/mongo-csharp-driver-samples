var coffeeApp = angular.module('coffeeApp', ['ngResource', 'ui.bootstrap']);

coffeeApp.factory('CoffeeOrder', function ($resource) {
    return $resource('/api/coffeeshop/:id/order/',
        { id: "@coffeeShopId" }, {});
});

coffeeApp.controller('DrinksController', function ($scope, CoffeeOrder) {
    $scope.types = [
        { name: "Americano", family: "Coffee" },
        { name: "Latte", family: "Coffee" },
        { name: "Cappucino", family: "Coffee" },
        { name: "Hot Chocolate", family: "Other" },
        { name: "Tea", family: "Other" },
    ];

    $scope.sizes = [
        "Small", 'Medium', 'Large'
    ];

    $scope.availableOptions = [
        { name: 'soy', appliesTo: 'milk' },
        { name: 'skim', appliesTo: 'milk' },
        { name: 'caramel', appliesTo: 'syrup' },
    ];

    $scope.messages = [];

    $scope.giveMeCoffee = function () {
        CoffeeOrder.save({ id: 1 }, $scope.drink, function (order) {
            $scope.messages.push({ type: "success", msg: "Order Sent!", coffeeShopId: order.CoffeeShopId, orderId: order.Id });
        });
    };

    $scope.closeAlert = function (index) {
        $scope.messages.slice(index, 1);
    };

    $scope.addOption = function () {
        if (!$scope.drink.selectedOptions) {
            $scope.drink.selectedOptions = [];
        }

        $scope.drink.selectedOptions.push($scope.newOption);
        $scope.newOption = '';
    };
});