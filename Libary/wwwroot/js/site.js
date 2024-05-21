// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Tăng số lượng sách trong giỏ hàng
$(".AddToCart_Btn").on("click", function(){
    let x = 0;
    x = 0 + Number($("#NumberItem_Cart").text());
    $("#NumberItem_Cart").text
    (
        (
            x + 1
        )
    );
});