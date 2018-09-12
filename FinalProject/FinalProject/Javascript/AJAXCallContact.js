//$(document).ready(function () {
//    $("#form").submit(onSubmit);
//});

function onCaseSubmit(e) {
    //e.preventDefault();
    //debugger;
    var q = buildQuery();
    //console.log(q);
    sendPost(q);
}

function buildQuery() {
    var priority = document.querySelector("#inputPriorityReason");
    if (priority.value == "") {
        var newText = "1," + document.querySelector("#inputSubject").value + "," + document.querySelector("#inputFirstName").value + "," + document.querySelector("#inputLastName").value + "," + document.querySelector("#inputPriority").value + "," + document.querySelector("#inputDescription").value
    }
    else {
        newText = document.querySelector("#inputSubject").value + "," + document.querySelector("#inputFirstName").value + "," + document.querySelector("#inputLastName").value + "," + document.querySelector("#inputPriority").value + "," + document.querySelector("#inputPriorityReason").value + "," + document.querySelector("#inputDescription").value
    }


    return newText;
}

function sendPost(newQuery) {
    var url = "https://mortgageproject.azurewebsites.net/API/case"/*My api*/;
    var query = newQuery;
    $.ajax({
        type: "POST",
        url: url,
        data: query,
        dataType: "text",
        success: function (responseText, status) {
            if (responseText != null) {
                alert(responseText);
            }
            else {
                alert("error");
            }
        },
        dataType: "json"
    })
}   