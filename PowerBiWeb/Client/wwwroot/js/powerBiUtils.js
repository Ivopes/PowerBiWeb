export function getMessage() {
    return 'Olá do Blazor!';
}

export  async function setMessage() {
    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    var exports = await getAssemblyExports("BlazorSample.dll");

    document.getElementById("result").innerText =
        exports.BlazorSample.JavaScriptInterop.Interop.GetMessageFromDotnet();
}

export  function convertArray(win1251Array) {
    var win1251decoder = new TextDecoder('windows-1251');
    var bytes = new Uint8Array(win1251Array);
    var decodedArray = win1251decoder.decode(bytes);
    console.log(decodedArray);
    return decodedArray;
};

export  function changeText(id, text) {
    document.getElementById(id).innerHTML = text;
}
export  function showReport(reportContainer, accessToken, embedUrl) {
    var models = window['powerbi-client'].models;
    console.log(reportContainer);
    console.log(accessToken);
    console.log(embedUrl);
    var config = {
        type: 'report',
        tokenType: models.TokenType.Embed,
        accessToken: accessToken,
        embedUrl: embedUrl,
        permissions: models.Permissions.All,
        settings: {
            filterPaneEnabled: true,
            navContentPaneEnabled: true
        }
    };
    // Embed the report and display it within the div container.
    powerbi.embed(reportContainer, config);
}

