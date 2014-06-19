var app = angular.module('PhotoApp', ['ngRoute']);

app.run(function ($rootScope, $location, account) {
    $rootScope.$on('$routeChangeSuccess', function () {
        account.renew(function (data) {
            if (data && data['success']) {
                $rootScope.username = data['username'];
            }
        });
    });

    $rootScope.logout = function () {
        account.logout(function (data) {
            if (data && data["success"]) {
                $rootScope.username = null;
                $location.path('/');
            }
        });
    }
});

app.factory('account', function ($http) {
    return {
        'login': function (username, password, remember, callback) {
            if (remember === undefined) remember = false;
            $http.post('/account/login', { username: username, password: password, remember: remember })
            .success(function (data, status, headers, config) {
                callback(data);
            });
        },
        'register': function (username, password, remember, callback) {
            if (remember === undefined) remember = false;
            $http.post('/account/register', { username: username, password: password, remember: remember })
            .success(function (data, status, headers, config) {
                callback(data);
            });
        },
        'renew': function (callback) {
            $http.get('/account/renew')
            .success(function (data, status, headers, config) {
                callback(data);
            });
        },
        'logout': function (callback) {
            $http.get('/account/logout')
            .success(function (data, status, headers, config) {
                callback(data);
            });
        }
    };
});

app.directive('pwCheck', [function () {
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            var firstPassword = '#' + attrs.pwCheck;
            elem.add(firstPassword).on('keyup', function () {
                scope.$apply(function () {
                    var v = elem.val() === $(firstPassword).val();
                    ctrl.$setValidity('pwmatch', v);
                });
            });
        }
    }
} ]);

app.config(['$routeProvider', function ($routeProvider, $location) {
    $routeProvider.
    when('/', {
        templateUrl: '/Content/html/index.html',
        controller: 'MainCtrl'
    }).
    when('/login', {
        templateUrl: '/Content/html/account/login.html',
        controller: 'LoginCtrl'
    }).
    when('/register', {
        templateUrl: '/Content/html/account/register.html',
        controller: 'RegisterCtrl'
    }).
    when('/photos/add', {
        templateUrl: '/Content/html/photos/add.html',
        controller: 'PhotosAddCtrl'
    }).
    otherwise({
        redirectTo: '/'
    });
} ]);

app.controller('LoginCtrl', function ($scope, $location, $rootScope, account) {
    $scope.model = {
        username: '',
        password: '',
        remember: false
    };

    $scope.cancel = function () {
        $location.path('/');
    };

    $scope.action = function () {
        account.login($scope.model.username, $scope.model.password, $scope.model.remember, function (data) {
            if (data && data["success"]) {
                $rootScope.username = data["username"];
                $location.path('/');
            }
        });
    };
});

app.controller('RegisterCtrl', function ($scope, $location, account) {
    $scope.model = {
        username: '',
        password: '',
        passwordConfirm: '',
        remember: false
    };

    $scope.cancel = function () {

    };

    $scope.action = function () {
        account.register($scope.model.username, $scope.model.password, $scope.model.remember, function (data) {
            
        });
    };
});

app.controller('MainCtrl', function () {

});

app.controller('PhotosAddCtrl', function () {

});