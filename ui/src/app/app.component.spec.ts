import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { Observable, of, throwError } from 'rxjs';
import { MockSearchApiService } from 'src/test/mock-search-api-service';
import { SearchApiService } from './api/search-api.service';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
    let fixture: ComponentFixture<AppComponent>;
    let component: AppComponent;
    let searchApiService: SearchApiService;
    let toastrService: ToastrService;
    let formBuilder: FormBuilder;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                AppComponent
            ],
            imports: [
                ToastrModule.forRoot(),
                FormsModule,
                ReactiveFormsModule,
                NoopAnimationsModule
            ],
            providers: [
                { provide: SearchApiService, useClass: MockSearchApiService }
            ]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AppComponent);
        searchApiService = TestBed.inject(SearchApiService);
        toastrService = TestBed.inject(ToastrService);
        formBuilder = TestBed.inject(FormBuilder);
        component = fixture.debugElement.componentInstance;
    });

    it('should create the app', () => {
        expect(component).toBeTruthy();
    });

    describe('doSearch', () => {
        it('should not do search if one already in progress', () => {
            component.searchInProgress = true;
            const searchSpy = spyOn(searchApiService, 'search')
                .and.callFake(() => of([]));

            component.doSearch();

            expect(searchSpy).not.toHaveBeenCalled();
        });

        it('should show error toastr when error from search api', () => {
            const expectedSearchText = 'test';
            const expectedToastMsg = 'Something went wrong. Please try again.';
            component.formGroup.setValue({search: expectedSearchText});
            const searchSpy = spyOn(searchApiService, 'search')
                .and.returnValue(throwError({status: 404}));
            const toastSpy = spyOn(toastrService, 'error')
                .and.callThrough();

            component.doSearch();

            expect(searchSpy).toHaveBeenCalledWith(expectedSearchText);
            expect(toastSpy).toHaveBeenCalledWith(expectedToastMsg);
            expect(component.latestSearchText).not.toEqual(expectedSearchText);
            expect(component.searchInProgress).toBeFalsy();
        });

        it('should set search results received from search api', () => {
            const expectedSearchText = 'test';
            component.formGroup.setValue({search: expectedSearchText});
            const expectedResults = [{ searchEngineName: 'Twitter', numberOfHits: 100000 }] as ISearchResult[];
            const searchSpy = spyOn(searchApiService, 'search')
                .and.returnValue(of(expectedResults));

            component.doSearch();

            expect(searchSpy).toHaveBeenCalledWith(expectedSearchText);
            expect(component.searchResults).toEqual(expectedResults);
            expect(component.latestSearchText).toEqual(expectedSearchText);
            expect(component.searchInProgress).toBeFalsy();
        });
    });
});
