$(() => {
    setInterval(() => {
        updateLikes();
    }, 1000);

    $("#like-button").on('click', function () {
        const id = $("#image-id").val();
        $.post('/home/IncreaseLikes', { id }, function () {
            updateLikes();
            $("#like-button").prop('disabled', true);
        });
    });

    function updateLikes() {
        const id = $("#image-id").val();
        $.get(`/home/GetLikes`, { id }, function ({ imageLikeNum }) {
            $("#likes-count").text(imageLikeNum);
            console.log(imageLikeNum);
        });
    }
});