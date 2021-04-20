@push('extra')
<div class="loading"></div>
<!-- media model area start -->
    	<div class="modal fade bd-example-modal-xl media-multiple" tabindex="-1" role="dialog" aria-labelledby="myExtraLargeModalLabel" aria-hidden="true">
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
    									   <form method="post" class="mediaform1" action="{{ route('admin.media.store') }}" enctype="multipart/form-data">
    											@csrf
    											<div class="media-header-link model-header-link f-right">
    												<a href="javascript:void(0)"><label for="mediaUp1"><i class="fas fa-cloud-upload-alt"></i></label></a>
    												<input type="file" name="media[]" id="mediaUp1"  class="media pull-left none mediaUp1" multiple="" >
                                                </div>
    									    </form>
    									</div>
    								</div>
    							</div>
    						</div>
    						<div class="row">
    							<div class="col-lg-9">
    								<div class="media-list model-media-list">
                                        <div  class="row popupmedia1">
    								</div>
                                     <input type="hidden" class="murl" value="{{ route('admin.medias.json') }}">
                                     <input type="hidden" class="multi_last">
                                    <p class="text-center"><button type="button"  class="view-more-button view-more-media1">{{ __('View more') }}</button></p>
    								</div>
    							</div>
    							<div class="col-lg-3">
                                    <div class="preview-show-rightbar preview-show1">
                                        <div class="preview-eye text-center">
                                            <i class="fas fa-eye"></i>
                                            <p>{{ __('No Preview Found') }}</p>
                                        </div>
                                    </div>
    								<div class="model-rightbar media-info-bar1 none">
    									<img class="img-fluid media-thumbnail" id="previewimg1"  src="" alt="">
    									<div class="modal-media-info">
    										<strong>{{ __('Name:') }}</strong>
    										<div><input type="text" id="img-name1" class="form-control"></div>
    										<strong>{{ __('Full Url:') }}</strong>
    										<div>
    											<input type="text" id="medialink1" value="" class="form-control">
    										</div>
    										<strong>{{ __('Size:') }}</strong>
    										<div><small id="size1"></small></div>
    										<strong>{{ __('Type:') }}</strong>
    										<div><small id="type1"></small></div>
    										<strong>{{ __('Uploaded At:') }}</strong>
    										<div><small id="upload1"></small></div>
    									</div>
    								</div>
    							</div>
    						</div>
    					</div>
    				</div>
    				<div class="modal-footer">
    					<button type="button" class="btn btn-secondary" data-dismiss="modal">{{ __('Close') }}</button>
    				<button type="button" class="btn btn-primary none use1" data-dismiss="modal">{{ __('Use') }}</button>
    			</div>
    		</div>
    	</div>
    </div>
<!-- media model area end -->
@endpush 
