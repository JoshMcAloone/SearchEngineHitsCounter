import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable()
export class SearchApiService {
    constructor(
        private readonly httpClient: HttpClient) { }

    public search(searchText: string): Observable<ISearchResult[]> {
        return this.httpClient.get<ISearchResult[]>(`${environment.apiEndpoint}search`, { params: { searchText } });
    }
}
