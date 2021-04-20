@section('extra')
<div class="loading"></div>
<!-- media model area start -->
<input type="hidden" id="base_url" value="{{ url('/') }}">
    	<div class="modal fade bd-example-modal-xl media-single" tabindex="-1" role="dialog" aria-labelledby="myExtraLargeModalLabel" aria-hidden="true">
    		<div class="modal-dialog modal-xl" role="document">
    			<div class="modal-content media-content media-model-content">
    				<div class="card media media-model">
    					<div class="card-body">
    						<div class="row">
    							<div class="col-lg-12">
    								<div class="row mb-8">
    									<div class="col-lg-6">
    										<div class="d-flex">
                                                <h4 class="mr-3">{{ __('Media List') }} </h4>
                                                <p class="errors"></p>
                                            </div>
                                        </div>
    									<div class="col-lg-6">
                                            @if($medialimit == true)
    									   <form method="post" class="mediaform" action="{{ route('admin.media.store') }}" enctype="multipart/form-data">
    											@csrf
    											<div class="media-header-link model-header-link f-right">
    												<a href="javascript:void(0)"><label for="mediaUp"><i class="fas fa-cloud-upload-alt"></i></label></a>
    												<input type="file" name="media[]" id="mediaUp"  class="media pull-left none mediaUp" multiple="" >
                                                </div>
    									    </form>
                                            @endif
    									</div>
    								</div>
    							</div>
    						</div>
    						<div class="row">
    							<div class="col-lg-9">
    								<div class="media-list model-media-list">
    									<div class="row popupmedia" id="">
                                         
    										
    									</div>
                                        <input type="hidden" id="murl" value="{{ route('admin.medias.json') }}">
                                            <input type="hidden" class="last_id1" >
                                    <p class="text-center"><button type="button"  class="view-more-button view-more-media" >{{ __('View more') }}</button></p>
    								</div>
    							</div>
    							<div class="col-lg-3">
                                    <div class="preview-show-rightbar">
                                        <div class="preview-eye text-center">
                                            <i class="fas fa-eye"></i>
                                            <p>{{ __('No Preview Found') }}</p>
                                        </div>
                                    </div>
    								<div class="model-rightbar media-info-bar none">
    									<img class="img-fluid media-thumbnail" id="previewimg"  src="" alt="">
    									<div class="modal-media-info">
    										<strong>{{ __('Name:') }}</strong>
    										<div><input type="text" id="img-name" class="form-control"></div>
    									   <strong>{{ __('Full Url:') }}</strong>
    								    <div>
    									<input type="text" id="medialink" value="" class="form-control">
    								</div>
    								<strong>{{ __('Size:') }}</strong>
    								<div><small id="size"></small></div>
    								<strong>{{ __('Type:') }}</strong>
    								<div><small id="type"></small></div>
    								<strong>{{ __('Uploaded At:') }}</strong>
    								<div><small id="upload"></small></div>	
    							</div>
    						</div>
    					</div>
    				</div>
    			</div>
    		</div>
    		<div class="modal-footer">
    			<button type="button" class="btn btn-secondary" data-dismiss="modal">{{ __('Close') }}</button>
    			<button type="button" class="btn btn-primary none use" data-dismiss="modal">{{ __('Use') }}</button>
    		</div>
    	</div>
    </div>
</div>
<!-- media model area end -->

@endsection        
