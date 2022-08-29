$(document).ready(function () {
    $('#searchCall').click(function () {
        if ($("#UrlText").val() == "")
            alert("Url cannot be empty")
        else {
            var urlRequest = {
                searchUrlText: $("#UrlText").val()
            };
            var request = $.ajax({
                type: "POST",
                url: "/Results/Results",
                data: JSON.stringify(urlRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }
            );
            //request.done(function (data){
                //console.log(data);
            /*});*/
            request.fail(function (jqXHR, textStatus) {
                console.log("Request failed: " + textStatus);
            });
        }
    });
});