var postList = document.getElementsByClassName("postimages");
var pageNumberHolder = [];

for (var i = 0; i < postList.length; i++) {
    var curPost = postList.item(i);
    pageNumberHolder[i] = 0;
    for (var j = 1; j < curPost.childElementCount; j++) {
        var curImg = curPost.children.item(j);  
        curImg.style.display = "none";
    }
}
console.log(pageNumberHolder);

function plusSlides(indexer, postIndex) {
    var curPage = pageNumberHolder[postIndex];
    var curPost = postList.item(postIndex);

    if (indexer < 0 && curPage > 0) {
        pageNumberHolder[postIndex] = curPage + indexer;
        for (var j = 0; j < curPost.childElementCount; j++) {
            var curImg = curPost.children.item(j);
            curImg.style.display = "none";
        }
        curPost.children.item(pageNumberHolder[postIndex]).style.display = "block";
    }

    if (indexer > 0 && pageNumberHolder[postIndex] < postList.item(postIndex).childElementCount - 1) {
        pageNumberHolder[postIndex] = curPage + indexer;
        for (var j = 0; j < curPost.childElementCount; j++) {
            var curImg = curPost.children.item(j);
            curImg.style.display = "none";
        }
        curPost.children.item(pageNumberHolder[postIndex]).style.display = "block";
    }
}

