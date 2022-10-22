function scrollToTargetAdjusted(sectionId) {
    const element = document.getElementById(sectionId);
    const headerOffset = 100;
    const elementPosition = element.getBoundingClientRect().top;
    const offsetPosition = elementPosition + window.pageYOffset - headerOffset;

    window.scrollTo({
        top: offsetPosition,
        behavior: "smooth"
    });
}

window.GotoSection = function (sectionId) {
    scrollToTargetAdjusted(sectionId);
    //document.getElementById(sectionId).scrollIntoView({ behavior: 'smooth' });
}