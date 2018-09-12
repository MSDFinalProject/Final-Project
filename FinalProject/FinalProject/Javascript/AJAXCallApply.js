function onSubmitApply(e) {
    console.log("here"); 
    //e.preventDefault();
    var q = textQ();
    console.log(q); 
    sendPost(q);
}

//function buildQuery() {
//    var newQuery = {
//        "query": document.querySelector("#inputMortgageAmount").value + "," + document.querySelector("#inputMortgageTerms").value + "," + document.querySelector("#inputRegion").value + "," + document.querySelector("#inputState").value
//    }
//    return newQuery;
//}

function textQ(){
    var newText = document.querySelector("#inputMortgageAmount").value + "," + document.querySelector("#inputMortgageTerms").value + "," + document.querySelector("#inputRegion").value + "," + document.querySelector("#inputState").value

    return newText;
}

function sendPost(textQ) {
    var url = "https://mortgageproject.azurewebsites.net/api/mortgage"/*My api*/;
    var user = textQ;
    $.ajax({
        type: "POST",
        url: url,
        data: user,
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
