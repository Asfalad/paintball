angular.module('pbService')
    .service("imageUpload", function ($http, $q) {

        return ({
            upload: upload
        });

        function upload(file) {
            var upl = $http({
                method: 'POST',
                url: '/papi/images', // /api/upload
                headers: {
                    'Content-Type': 'multipart/form-data'
                },
                data: file,
                transformRequest: function (data, headersGetter) {
                    var formData = new FormData();
                    angular.forEach(data, function (value, key) {
                        formData.append(key, value);
                    });

                    var headers = headersGetter();
                    delete headers['Content-Type'];

                    return formData;
                }
            });
            return upl.then(handleSuccess, handleError);

        } // End upload function

        // ---
        // PRIVATE METHODS.
        // ---

        function handleError(response, data) {
            if (!angular.isObject(response.data) || !response.data.message) {
                return ($q.reject("An unknown error occurred."));
            }
            alert(JSON.stringify(response));
            return ($q.reject(response.data.message));
        }

        function handleSuccess(response) {
            return (response);
        }

    })
  .directive("fileinput", [function () {
      return {
          scope: {
              fileinput: "=",
              filepreview: "="
          },
          link: function (scope, element, attributes) {
              element.bind("change", function (changeEvent) {
                  scope.fileinput = changeEvent.target.files[0];
                  var reader = new FileReader();
                  reader.onload = function (loadEvent) {
                      scope.$apply(function () {
                          scope.filepreview = loadEvent.target.result;
                      });
                  }
                  reader.readAsDataURL(scope.fileinput);
              });
          }
      }
  }]);
  