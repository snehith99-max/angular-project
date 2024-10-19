import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsMstReorderleveladdComponent } from './ims-mst-reorderleveladd.component';

describe('ImsMstReorderleveladdComponent', () => {
  let component: ImsMstReorderleveladdComponent;
  let fixture: ComponentFixture<ImsMstReorderleveladdComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsMstReorderleveladdComponent]
    });
    fixture = TestBed.createComponent(ImsMstReorderleveladdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
