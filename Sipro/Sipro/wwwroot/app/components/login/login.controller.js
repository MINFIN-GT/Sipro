/**
 * 
 */

var app = angular.module('sipro', [
	'ngTouch','ngUtilidades'
]);

app.controller('loginController', [
	'$rootScope','$scope','$http','Utilidades','$window',function($rootScope,$scope,$http,$utilidades,$window){
		
		$window.document.title = $utilidades.sistema_nombre + ' - Login';
		var mi = this;
		mi.sistema_nombre =  $utilidades.sistema_nombre;
		mi.username = "";
		mi.password = "";
		$rootScope.etiquetas = null;
		$scope.showerror = false;
		mi.login=function(){
			if(this.username!='' && this.password!=''){
				var data = { username: this.username, password: this.password};
                var headers = { headers: { RequestVerificationToken: document.getElementsByName('__RequestVerificationToken')[0].value }};
				$http.post('/api/Login/In', data, headers).then(function(response){
					if(response.data.success==true)
					   	window.location.href = '/main';
					else
					    $scope.showerror = true;
				 	}, function errorCallback(response){
				 		$scope.showerror = true;
				 	}
				 );
			 }
		 }
	}
]);

