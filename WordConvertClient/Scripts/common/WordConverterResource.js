(function () {
    "use strict";

    angular
        .module("common.services")
        .factory("wordConverterResource",
            [
                "$resource",
                "appSettings",
                wordConverterResource
            ]);

    function wordConverterResource($resource, appSettings) {
        return $resource(appSettings.serverPath + "/api/Converter/:number");
    }
}());