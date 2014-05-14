var coffeeApp = angular.module('coffeeApp', ['ngResource']);

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
    ]

    $scope.giveMeCoffee = function () {
        CoffeeOrder.save({ id: 1 }, $scope.drink);
    }
});