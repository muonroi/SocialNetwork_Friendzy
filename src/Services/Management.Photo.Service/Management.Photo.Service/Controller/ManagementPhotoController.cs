namespace Management.Photo.Service.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class ManagementPhotoController(IMediator mediator, IWebHostEnvironment hostingEnvironment) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResult<StoreInfoDTO>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetResourceByType([FromQuery] GetResourceByIdQuery request)
    {
        ApiResult<StoreInfoDTO> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("type")]
    [ProducesResponseType(typeof(ApiResult<IEnumerable<StoreInfoDTO>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetResourceByType([FromQuery] GetResourceQuery request)
    {
        ApiResult<IEnumerable<StoreInfoDTO>> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    #region Command

    [HttpPost("import")]
    [ProducesResponseType(typeof(ApiResult<ImportResourceCommandResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ImportResourceByType([FromForm] ImportResourceCommand request)
    {
        ApiResult<ImportResourceCommandResponse> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("import/multiple")]
    [ProducesResponseType(typeof(ApiResult<IEnumerable<ImportMultipleResourceCommandResponse>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ImportMultipleResource([FromForm] ImportMultipleResourceCommand request)
    {
        ApiResult<IEnumerable<ImportMultipleResourceCommandResponse>> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("upload-single")]
    public async Task<IActionResult> UploadSingleImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        string uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "images");

        if (!Directory.Exists(uploadsFolderPath))
        {
            _ = Directory.CreateDirectory(uploadsFolderPath);
        }

        string filePath = Path.Combine(uploadsFolderPath, file.FileName);

        using (FileStream stream = new(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        string url = $"{Request.Scheme}://{Request.Host}/images/{file.FileName}";

        return Ok(new { url });
    }

    [HttpPost("upload-multiple")]
    public async Task<IActionResult> UploadMultipleImages(List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest("No files uploaded.");
        }

        string uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "images");

        if (!Directory.Exists(uploadsFolderPath))
        {
            _ = Directory.CreateDirectory(uploadsFolderPath);
        }

        List<string> urls = [];

        foreach (IFormFile file in files)
        {
            string filePath = Path.Combine(uploadsFolderPath, file.FileName);

            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string url = $"{Request.Scheme}://{Request.Host}/images/{file.FileName}";
            urls.Add(url);
        }

        return Ok(new { urls });
    }

    #endregion Command
}