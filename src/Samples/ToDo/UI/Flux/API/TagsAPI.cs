﻿namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using Samples.ToDo.Shared;

#endregion

public class TagsAPI : ApiBase
{
    #region Constructors

    public TagsAPI(HttpClient http,
                   IDispatcher dispatcher,
                   IState<LanguageState> localizationState)
            : base(http, dispatcher, localizationState) { }

    #endregion

    public async Task<TagDto[]> GetAsync(int[] ids,
                                         string accessToken,
                                         string validationKey,
                                         CancellationToken cancellationToken = default)
    {
        var uri = $"{ApiRoutes.ReadTags}?{ids.ToApiParams(ApiRoutes.Params.TagsIds)}";

        var result = await this.Http.GetApiResponseOrDefaultAsync<TagDto[]>(dispatcher: this.dispatcher,
                                                                            acceptLanguage: this.localizationState.Value.Language,
                                                                            validationKey: validationKey,
                                                                            httpMethod: HttpMethodType.GET,
                                                                            uri: uri,
                                                                            accessToken: accessToken,
                                                                            cancellationToken: cancellationToken);

        return result ?? Array.Empty<TagDto>();
    }
}