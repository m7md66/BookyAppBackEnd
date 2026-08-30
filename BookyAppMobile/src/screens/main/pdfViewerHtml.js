import { colors } from '../../theme';

const PDFJS_VERSION = '3.11.174';
const PDFJS_CDN = `https://cdnjs.cloudflare.com/ajax/libs/pdf.js/${PDFJS_VERSION}`;

export function buildPdfViewerHtml(pdfUrl, strings = {}) {
  const safeUrl = JSON.stringify(pdfUrl);
  const s = {
    prev: strings.prev ?? 'Prev',
    next: strings.next ?? 'Next',
    loadingBook: strings.loadingBook ?? 'Loading book…',
    postAsQuote: strings.postAsQuote ?? 'Post as Quote',
    failedToLoad: strings.failedToLoad ?? 'Failed to load book',
  };
  return `<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
  <style>
    html, body { margin: 0; padding: 0; background: ${colors.background}; overflow-x: hidden; }
    #toolbar { position: fixed; top: 0; left: 0; right: 0; height: 48px; background: ${colors.card}; border-bottom: 1px solid ${colors.border};
      display: flex; align-items: center; justify-content: center; gap: 10px; z-index: 20; }
    #toolbar button { background: ${colors.primary}; color: ${colors.onPrimary}; border: none; border-radius: 6px; padding: 6px 12px; font-size: 14px; }
    #toolbar button:disabled { opacity: 0.4; }
    #page-num { width: 44px; text-align: center; border: 1px solid ${colors.border}; border-radius: 6px; padding: 4px; font-size: 14px; }
    #page-count-label { font-size: 14px; color: ${colors.textTertiary}; }
    #viewer { margin-top: 56px; display: flex; justify-content: center; padding-bottom: 40px; }
    #page-container { position: relative; }
    #text-layer { position: absolute; top: 0; left: 0; overflow: hidden; line-height: 1; }
    #text-layer span { position: absolute; white-space: pre; color: transparent; cursor: text; }
    #text-layer span::selection { background: rgba(47,93,80,0.30); }
    #quote-btn { position: absolute; display: none; z-index: 50; background: ${colors.primary}; color: ${colors.onPrimary}; border: none;
      border-radius: 8px; padding: 8px 14px; font-size: 13px; box-shadow: 0 2px 8px rgba(0,0,0,0.25); }
    #status { text-align: center; padding: 40px 16px; color: ${colors.textSecondary}; font-size: 14px; }
  </style>
</head>
<body>
  <div id="toolbar">
    <button id="prev-btn">${s.prev}</button>
    <input id="page-num" type="number" value="1" />
    <span id="page-count-label">/ <span id="page-count">-</span></span>
    <button id="next-btn">${s.next}</button>
  </div>
  <div id="viewer">
    <div id="status">${s.loadingBook}</div>
    <div id="page-container" style="display:none;">
      <canvas id="pdf-canvas"></canvas>
      <div id="text-layer"></div>
    </div>
  </div>
  <button id="quote-btn">${s.postAsQuote}</button>

  <script src="${PDFJS_CDN}/pdf.min.js"></script>
  <script>
    pdfjsLib.GlobalWorkerOptions.workerSrc = "${PDFJS_CDN}/pdf.worker.min.js";

    var pdfUrl = ${safeUrl};
    var pdfDoc = null;
    var pageNum = 1;
    var pageRendering = false;
    var pageNumPending = null;

    var canvas = document.getElementById('pdf-canvas');
    var ctx = canvas.getContext('2d');
    var textLayerDiv = document.getElementById('text-layer');
    var statusEl = document.getElementById('status');
    var pageContainer = document.getElementById('page-container');
    var quoteBtn = document.getElementById('quote-btn');

    function renderTextLayer(page, viewport) {
      textLayerDiv.innerHTML = '';
      textLayerDiv.style.width = viewport.width + 'px';
      textLayerDiv.style.height = viewport.height + 'px';
      page.getTextContent().then(function (textContent) {
        textContent.items.forEach(function (item) {
          var tx = pdfjsLib.Util.transform(viewport.transform, item.transform);
          var fontHeight = Math.hypot(tx[2], tx[3]);
          var span = document.createElement('span');
          span.textContent = item.str;
          span.style.left = tx[4] + 'px';
          span.style.top = (tx[5] - fontHeight) + 'px';
          span.style.fontSize = fontHeight + 'px';
          textLayerDiv.appendChild(span);
        });
      });
    }

    function renderPage(num) {
      pageRendering = true;
      pdfDoc.getPage(num).then(function (page) {
        var unscaled = page.getViewport({ scale: 1 });
        var scale = (window.innerWidth - 16) / unscaled.width;
        var viewport = page.getViewport({ scale: scale });
        canvas.width = viewport.width;
        canvas.height = viewport.height;
        var renderTask = page.render({ canvasContext: ctx, viewport: viewport });
        renderTask.promise.then(function () {
          pageRendering = false;
          if (pageNumPending !== null) {
            renderPage(pageNumPending);
            pageNumPending = null;
          }
        });
        renderTextLayer(page, viewport);
      });
      document.getElementById('page-num').value = num;
    }

    function queueRenderPage(num) {
      if (pageRendering) {
        pageNumPending = num;
      } else {
        renderPage(num);
      }
    }

    document.getElementById('prev-btn').addEventListener('click', function () {
      if (pageNum <= 1) return;
      pageNum--;
      queueRenderPage(pageNum);
    });
    document.getElementById('next-btn').addEventListener('click', function () {
      if (!pdfDoc || pageNum >= pdfDoc.numPages) return;
      pageNum++;
      queueRenderPage(pageNum);
    });
    document.getElementById('page-num').addEventListener('change', function (e) {
      var num = parseInt(e.target.value, 10);
      if (pdfDoc && !isNaN(num) && num >= 1 && num <= pdfDoc.numPages) {
        pageNum = num;
        queueRenderPage(pageNum);
      }
    });

    document.addEventListener('selectionchange', function () {
      var sel = window.getSelection();
      var text = sel.toString().trim();
      if (text.length > 0) {
        var range = sel.getRangeAt(0);
        var rect = range.getBoundingClientRect();
        quoteBtn.style.display = 'block';
        quoteBtn.style.left = Math.max(8, rect.left) + 'px';
        quoteBtn.style.top = Math.max(8, rect.top - 44 + window.scrollY) + 'px';
        quoteBtn.dataset.text = text;
      } else {
        quoteBtn.style.display = 'none';
      }
    });
    quoteBtn.addEventListener('click', function () {
      var text = quoteBtn.dataset.text;
      if (text && window.ReactNativeWebView) {
        window.ReactNativeWebView.postMessage(JSON.stringify({ type: 'quote', text: text }));
        window.getSelection().removeAllRanges();
        quoteBtn.style.display = 'none';
      }
    });

    pdfjsLib.getDocument(pdfUrl).promise.then(function (doc) {
      pdfDoc = doc;
      document.getElementById('page-count').textContent = doc.numPages;
      statusEl.style.display = 'none';
      pageContainer.style.display = 'block';
      renderPage(pageNum);
    }).catch(function (err) {
      statusEl.textContent = ${JSON.stringify(s.failedToLoad)} + ': ' + (err && err.message ? err.message : err);
      statusEl.style.color = ${JSON.stringify(colors.danger)};
    });
  </script>
</body>
</html>`;
}
