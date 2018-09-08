$(document).ready(function () {
    $("#form").submit(onSubmit);
});

function onSubmit(e) {
    e.preventDefault();
    //debugger;
    var q = buildQuery();
    console.log(q);
    sendPost(q);
}

function buildQuery() {
    var newQuery = {
        "query": document.querySelector("#fname").value + ", " + document.querySelector("#lname").value + " " + document.querySelector("#email").value + " " + document.querySelector("#msg").value
    }

    return newQuery;
    //sendPost(newPerson);
}

function sendPost(newQuery) {
    var url = "https://contactwebapp.azurewebsites.net/API/Person"/*My api*/;
    var query = newQuery;
    $.ajax({
        type: "POST",
        url: url,
        data: query,
        dataType: "JSON",
        success: function (response) {
            if (response != null) {
                para.text(JSON.stringify(response));
            }
            else {
                alert("error");
            }
        }
    })
}   