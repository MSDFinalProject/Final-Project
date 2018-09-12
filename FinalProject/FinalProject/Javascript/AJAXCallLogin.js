function onSubmit(e) {
    //e.preventDefault();
    //debugger;
    var q = buildQuery();
    console.log(q);
    sendPost(q);
}

function buildQuery() {
    var newQuery = {
        "query": document.querySelector("#inputEmail").value + "," + document.querySelector("#inputPassword").value
    }
    return newQuery;
}

function sendPost(newQuery) {
    var url = "https://mortgageproject.azurewebsites.net/api/login"/*My api*/;
    var user = newQuery;
    $.ajax({
        type: "GET",
        url: url,
        data: user,
        dataType: "json",
        success: function (responseText, status) {
            if (responseText != null) {
                if (status == "success") {
                    window.location.href = "http://localhost:49491/Pages/ManageMorgagePage.html";
                    //window.location.href = "https://mortgageproject.azurewebsites.net/Pages/ManageMorgagePage.html";
                }
                else {
                    //Custom Validation for password
                }
            }
            else {
                alert("error");
            }
        }
    })
}   
