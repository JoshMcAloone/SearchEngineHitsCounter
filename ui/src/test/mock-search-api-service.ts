import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MockSearchApiService {
    public search(searchText: string): Observable<ISearchResult[]> {
        return of([]);
    }
}
