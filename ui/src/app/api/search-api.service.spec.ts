import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed, async, getTestBed } from '@angular/core/testing';
import { environment } from 'src/environments/environment';
import { SearchApiService } from './search-api.service';

describe('SearchApiService', () => {
    let injector: TestBed;
    let service: SearchApiService;
    let httpMock: HttpTestingController;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                HttpClientTestingModule
            ],
            providers: [
                SearchApiService
            ]
        });
        injector = getTestBed();
        service = TestBed.inject(SearchApiService);
        httpMock = TestBed.inject(HttpTestingController);
    }));

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    describe('search', () => {
        it('should call search api when performing search', () => {
            const expectedSearchText = 'test';
            const restResponse = [];

            service.search(expectedSearchText)
                .subscribe((response) => {
                    expect(response).toEqual(restResponse as ISearchResult[]);
                });

            const req = httpMock.expectOne(`${environment.apiEndpoint}search?searchText=${expectedSearchText}`);
            expect(req.request.method).toBe('GET');
        });
    });
});
