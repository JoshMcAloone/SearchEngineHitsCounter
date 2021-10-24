import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { SearchApiService } from './api/search-api.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
    public searchResults: ISearchResult[];
    public latestSearchText: string;
    public searchInProgress = false;

    public formGroup = this.formBuilder.group({
        search: ['', [Validators.required, Validators.maxLength(100)]]
    });

    constructor(
        private readonly toastr: ToastrService,
        private readonly formBuilder: FormBuilder,
        private readonly searchApiService: SearchApiService) { }

    public doSearch(): void {
        if (this.searchInProgress) {
            return;
        }

        this.searchInProgress = true;

        const searchText = this.formGroup.get('search').value;

        this.searchApiService.search(searchText)
            .subscribe(
                res => this.searchResults = res,
                err => this.handleError(),
                () => this.handleCompletedAction(searchText)
            );
    }

    private handleError(): void {
        this.toastr.error('Something went wrong. Please try again.');
        this.searchInProgress = false;
    }

    private handleCompletedAction(searchedText: string): void {
        this.latestSearchText = searchedText;
        this.searchInProgress = false;
    }
}
