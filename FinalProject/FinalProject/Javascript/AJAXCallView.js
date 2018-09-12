$(document).ready(function () {
    getPost();
});

function onSubmit(e) {
    //e.preventDefault();
    //debugger;
    var q = buildQuery();
    //console.log(q);
    sendGet(q);
}

function buildQuery(getData) {
    var table = document.getElementById("table");
    var mortgageNums = [];
    var rows = [];
    var cells = [];
    var i;
    var j = 0;
    console.log(getData);
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
    console.log(rows[0]);
    for (i = 0; i < rows.length; i++) {
        var tr = document.createElement("tr");
        var content = rows[i];
        console.log(rows[i] + " " + i);
        string = "";
        for (j = 0; j < content.length + 1; j++) {
            var k = 0;
            //if (k == 0) {
            //    mortgageNums[k]
            //}
            if (content[j] == ',') {
                cells[k] = string;
                console.log(string);
                var td = document.createElement("td");
                var txt = document.createTextNode(cells[k]);
                td.appendChild(txt);
                tr.appendChild(td);
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
    var url = "https://mortgageproject.azurewebsites.net/API/case"/*My api*/;
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
