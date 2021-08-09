// import Something from "./another-js-file.js";

class App
{    
    constructor() {
        // setup         
    }

    go() {
        // retrieve and display the list of books
        Books.init();
    }
}

$(function () {    
    new App().go();
});

var Books = {
    _baseUrl: "api/books/",
    _bookId: "",
    _bookTitle: "",
    init: function () {    
        $("#btnSearch").click(this.onSearch);
        $("#txtSearch").keyup(this.onSearch);
        
        $.ajax({
            url: this._baseUrl,
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var html = '';
                $.each(result, function (key, item) {
                    html += "<a href='#' onclick='javascript:Books.onBookSelect(" + item.Id + ",\"" + item.Title + "\");' class='list-group-item list-group-item-action'>";
                    html += item.Title + "</a>";                    
                });
                
                $('#divBookList').html(html);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    },
    onBookSelect: function (bookId, bookTitle) {
        this._bookId = bookId;
        this._bookTitle = bookTitle;

        $("#txtSearch").val("")
        
        $('#divWordHeader').html("Most common words in '" + bookTitle + "'");

        $.ajax({
            url: this._baseUrl + bookId,
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var html = '';
                $.each(result, function (key, item) {
                    html += "<tr><td>" + item.Name + "</td>"
                    html += "<td>" + item.Count + "</td></tr>"                    
                });

                $('#tblWords').html(html);                
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });        
    },
    onSearch: function () {    
        if (!Books._bookId) return;

        var searchText = $("#txtSearch").val();

        if (searchText.length < 3) return;

        $('#divWordHeader').html("Words in '" + Books._bookTitle + "' starting with '" + searchText + "'");

        $.ajax({
            url: Books._baseUrl + Books._bookId + "?searchText=" + searchText,
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                var html = '';
                $.each(result, function (key, item) {
                    html += "<tr><td>" + item.Name + "</td>"
                    html += "<td>" + item.Count + "</td></tr>"
                });

                $('#tblWords').html(html);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}