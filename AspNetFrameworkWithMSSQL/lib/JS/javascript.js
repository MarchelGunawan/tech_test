$(document).ready(function () {
    $("#dataTable").DataTable();

    $("#start_date").change(function () {
        var tmp = $(this).val().split(" ");
        var date = tmp[0];
        $("#end_date").prop("min", date);
    })
    $("#end_date").change(function () {
        var tmp = $(this).val().split(" ");
        var date = tmp[0];
        $("#start_date").prop("max", date);
    })

    $("#search_data").click(function () {
        var start_date = $("#start_date").val().split(" ")[0];
        var end_date = $("#end_date").val().split(" ")[0];
        var title = $("#Title").val();
        var Category = $("#Category").val();
        $.ajax({
            type: "POST",
            url: 'Test/searchData',
            data: {
                Start_date: start_date,
                End_date: end_date,
                title: title,
                Category: Category
            },
            success: function (data) {
                var table = $("#dataTable").DataTable();
                table.clear().draw();
                var res = "";
                for (const i of data) {
                    res += "<tr><td><a href='' class='picker' >" + i["title"] + "</a></td><td>" + i["Category"] + "</td><td>" + i["Start_date"] + "</td><td>" + i["Location"] + "</td></tr>";
                }
                table.row.add($(res)).draw();
            }
        })
    })
})