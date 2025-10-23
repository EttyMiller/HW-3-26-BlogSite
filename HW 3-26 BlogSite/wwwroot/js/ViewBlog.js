

$(() => {
    $("input").on("input", function() {
        isFormValid();
    })

    $("textarea").on("input", function() {
        isFormValid();
    })

    function isFormValid() {
        const name = $("#name").val().trim();
        const content = $("#content").val().trim();

        $("#submit").prop("disabled", !(name && content))
    }
})