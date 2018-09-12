$(document).ready(function () {
    getPost();
});



function buildQuery(getData) {
    var optionset = document.getElementById("displayMortgages"); 
    var table = document.getElementById("mortgagesTable"); 
    var mortgageNums = [];
    var rows = []; 
    var cells = []; 
    var i;
    var j = 0;
    var string = ""; 
    for (i = 0; i < getData.length; i++) {
        if (getData[i] == ';') {
            rows[j] = string; 
            j++;
            string = "";
            continue;
        } else {
            string += getData[i]; 
        }
    }
    for (i = 0; i < rows.length; i++) {
        var tr = document.createElement("tr");
        var content = rows[i];
        string = "";
        var k = 0; 
        for (j = 0; j < content.length + 1; j++) {
            if (content[j] == ',') {
                cells[k] = string;
                var td = document.createElement("td");
                var txt = document.createTextNode(cells[k]);
                td.appendChild(txt);
                tr.appendChild(td);
                if (k === 0) {
                    var opt = document.createElement("option");
                    opt.value = cells[k]; 
                    opt.text = cells[k]; 
                    optionset.appendChild(opt); 
                }
                k++; 
                string = "";
                continue;
            } else {
                string += content[j];
            }
        }
        table.appendChild(tr); 
    }
}

function getPost() {
    var url = "https://mortgageproject.azurewebsites.net/api/mortgage"/*My api*/;
    //var user = newQuery;
    $.ajax({
        type: "GET",
        url: url,
        dataType: "json",
        success: function (responseText, status) {
            if (responseText != null) {
                if (status == "success") {
                    buildQuery(responseText); 
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



//<--POST--> 


function onSubmitMortgage(e) {
    var q = buildPostQuery();
    sendPost(q);
}

function buildPostQuery() {
    var newQuery = document.getElementById("displayMortgages").value
    return newQuery;
}

function sendPost(newQuery) {
    //This is where the mortgage payment controller api would go
    var url = "https://mortgageproject.azurewebsites.net/api/login"/*My api*/;
    var user = newQuery;
    $.ajax({
        type: "GET",
        url: url,
        data: user,
        dataType: "json",
        success: function (responseText, status) {
            if (responseText != null) {
                alert(responseText);
            }
            else {
                alert("error");
            }
        }
    })
}   
