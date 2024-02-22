﻿namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using Samples.ToDo.Shared;

#endregion

public class ProjectsAPI : ApiBase
{
    #region Constructors

    public ProjectsAPI(HttpClient http,
                       IDispatcher dispatcher,
                       IState<LocalizationState> localizationState)
            : base(http, dispatcher, localizationState) { }

    #endregion

    public async Task<PaginatedResponseDto<ProjectEditableDto>> GetAsync(string searchTerm,
                                                                         int page,
                                                                         string accessToken,
                                                                         string validationKey,
                                                                         int[] tagsIds = default,
                                                                         CancellationToken cancellationToken = default)
    {
        var uri = $"{ApiRoutes.GetProjects}?"
                + $"{ApiRoutes.Params.SearchTerm}={searchTerm}&"
                + $"{ApiRoutes.Params.page}={page}&";

        if (tagsIds?.Any() == true)
            uri += $"&{tagsIds.ToApiParams(ApiRoutes.Params.TagsIds)}";

        var result = await this.Http.GetApiResponseOrDefaultAsync<PaginatedResponseDto<ProjectEditableDto>>(dispatcher: this.dispatcher,
                                                                                                            acceptLanguage: this.localizationState.Value.Language,
                                                                                                            validationKey: validationKey,
                                                                                                            httpMethod: HttpMethodType.GET,
                                                                                                            uri: uri,
                                                                                                            accessToken: accessToken,
                                                                                                            cancellationToken: cancellationToken);

        return result ?? new PaginatedResponseDto<ProjectEditableDto>();
    }

    public async Task<bool> CreateAsync(CreateProjectRequest request,
                                        string accessToken,
                                        string validationKey,
                                        CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<bool>(dispatcher: this.dispatcher,
                                                                  acceptLanguage: this.localizationState.Value.Language,
                                                                  validationKey: validationKey,
                                                                  httpMethod: HttpMethodType.POST,
                                                                  uri: ApiRoutes.CreateProject,
                                                                  accessToken: accessToken,
                                                                  content: request,
                                                                  cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateAsync(UpdateProjectRequest request,
                                        string accessToken,
                                        string validationKey,
                                        CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<bool>(dispatcher: this.dispatcher,
                                                                  acceptLanguage: this.localizationState.Value.Language,
                                                                  validationKey: validationKey,
                                                                  httpMethod: HttpMethodType.PUT,
                                                                  uri: ApiRoutes.UpdateProject,
                                                                  accessToken: accessToken,
                                                                  content: request,
                                                                  cancellationToken: cancellationToken);
    }
}