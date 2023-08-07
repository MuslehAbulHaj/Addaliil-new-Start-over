import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  paginationResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>;

  constructor(private http:HttpClient) { }

  getMembers(userPrams: UserParams){
    //getMembers(page?: number, itemsPerPage?: number){
    //if (this.members.length > 0) return of (this.members);
    //return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      // map(members => {
      //   this.members = members;
      //   return members;
      // })
    //)
    let params = this.getPaginationHeaders(userPrams.pageNumber, userPrams.pageSize);
    
    params = params.append('minAge', userPrams.minAge);
    params = params.append('maxAge', userPrams.maxAge);
    params = params.append('gender', userPrams.gender);

    return this.getPaginationResults<Member[]>(this.baseUrl + 'users', params)
  }

  private getPaginationResults<T>(url:string , params: HttpParams) {
    const paginationResult: PaginatedResult<T> = new PaginatedResult<T>;
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        if (response.body) {
          paginationResult.result = response.body;
        }

        const pagination = response.headers.get('Pagination');
        if (pagination) {
          paginationResult.pagination = JSON.parse(pagination);
        }
        return paginationResult;
      })
    );
  }

  private getPaginationHeaders(pageNumber: number , pageSize: number) {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
    return params;
  }

  getMember(username: string){
    const member = this.members.find(x => x.userName === username);
    if (member) return of (member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username)
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map( () => {
        const index = this.members.indexOf(member);
        this.members[index] = {...this.members[index] , ...member}
      })
    )
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }
}

