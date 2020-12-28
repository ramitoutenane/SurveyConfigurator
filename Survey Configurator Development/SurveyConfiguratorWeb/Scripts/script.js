let Data = {};
let SortedData = {};
let SortOption = null;

function GetQuestionList(pHash) {
    $.ajax({
        url: '/Survey/QuestionList?Hash=' + pHash,
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            if (data != null) {
                Data = data;
                SortTable(SortOption);
            }
        },
        error: function () {
            ShowTableError();
        }
    });
}

function PopulateTable(pData) {
    if (pData == null)
        return;
    $("#loader").removeClass("hidden")
    $("#TableError").addClass("hidden");
    $("#QuestionTable").addClass("hidden");
    let tTableBody = $("#QuestionTableBody")[0];
    tTableBody.innerHTML = '';
    for (let i = 0; i < pData.length; i++) {
        let tRow = `
                    <tr>
                        <td>${pData[i].Text}</td>
                        <td>${pData[i].Order}</td>
                        <td>${pData[i].TypeString}</td>
                        <td>
                            <a href="/Survey/Edit/${pData[i].Id}" class="edit"  data-toggle="tooltip" > <i class="material-icons">&#xE254;</i></a >
                            <a href="#" onclick= ShowDeleteDialog(${pData[i].Id}); class="delete" data-toggle="tooltip"><i class="material-icons">&#xE872;</i></a>
                        </td>
                    </tr>
                    `
        tTableBody.innerHTML += tRow;
    }
    $("#loader").addClass("hidden")
    $("#QuestionTable").removeClass("hidden");
};

function ShowTableError() {
    $("#TableError").removeClass("hidden");
    $("#loader").addClass("hidden")
    $("#QuestionTable").addClass("hidden");
}

function StartAutoRefresh(pInterval) {
    if (pInterval == null || isNaN(pInterval))
        pInterval = 20000;
    window.setInterval(function () {
        let tHash = md5((JSON.stringify(Data))).toUpperCase();
        GetQuestionList(tHash);
    }, pInterval);
}


function SortTable(pSortOption) {
    if (pSortOption == null)
         PopulateTable(Data);
    else {
        $("#QuestionTable").addClass("hidden");
        $("#loader").removeClass("hidden")
        SortOption = pSortOption;
        tSortMethod = SortOption.value
        SortedData = Data.sort((a, b) => a[tSortMethod] - b[tSortMethod])
        PopulateTable(SortedData)
    }
}

function ChangeLanguage(pSelectedLanguageOption) {
    if (pSelectedLanguageOption != null && pSelectedLanguageOption.value != null) {
        document.location.href = `/Survey/SetLanguage?Language=${pSelectedLanguageOption.value}`;
    }
}