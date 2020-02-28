var postList = document.getElementsByClassName("postimages");
var pageNumberHolder = [];

for (var i = 0; i < postList.length; i++) {
    var widestImage = 0;
    var curPost = postList.item(i);
    console.log(curPost);
    pageNumberHolder[i] = 0;

    //for (var j = 0; j < curPost.childElementCount - 1; j++) {
    //    var curImg = curPost.children.item(j);
    //    console.log(curImg.width)
    //    console.log(curImg.style.display);
    //    if (curImg.width > widestImage) {
    //        widestImage = curImg.width;
    //    }
    //}

    for (var j = 1; j < curPost.childElementCount; j++) {
        var curImg = curPost.children.item(j); 
        curImg.style.display = "none";
    }
}

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

