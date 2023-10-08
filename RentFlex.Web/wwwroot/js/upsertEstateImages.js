const selectedImages = [];

function displaySelectedFiles(event) {
    const fileList = event.target.files;
    const imagePreview = document.getElementById('imagePreview');

    for (let i = 0; i < fileList.length; i++) {
        const file = fileList[i];
        const reader = new FileReader();

        reader.onload = function (e) {
            const img = document.createElement('img');
            img.src = e.target.result;
            img.style.maxWidth = '100px';
            img.style.maxHeight = '100px';
            imagePreview.appendChild(img);
        };

        reader.readAsDataURL(file);
    }

    Array.prototype.push.apply(selectedImages, fileList);
}

document.getElementById('uploadForm').addEventListener('submit', function (event) {
    event.preventDefault();

    const formData = new FormData();

    for (let i = 0; i < selectedImages.length; i++) {
        formData.append('images', selectedImages[i]);
    }

    fetch('your_upload_url', {
        method: 'POST',
        body: formData
    })
    .then(response => response.json())
    .then(data => {
        console.log('Upload successful:', data);
    })
    .catch(error => console.error('Error:', error));
});