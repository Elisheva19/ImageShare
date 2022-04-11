$(() => {
    const id = $('#image-id').val();

    $('#like-button').on('click', function () {
       

        $.post('/home/update', { id }, () => {
            $('#like-button').attr('disabled', 'true')
          
        })
    })

    setInterval(() => {
        $.get('/home/getlikes', { id }, function (likes) {
            $("#likes-count").text(likes);
        })
        }, 500)
    })


