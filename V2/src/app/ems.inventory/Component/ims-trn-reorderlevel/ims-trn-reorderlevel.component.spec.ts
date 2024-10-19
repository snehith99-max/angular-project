import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnReorderlevelComponent } from './ims-trn-reorderlevel.component';

describe('ImsTrnReorderlevelComponent', () => {
  let component: ImsTrnReorderlevelComponent;
  let fixture: ComponentFixture<ImsTrnReorderlevelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnReorderlevelComponent]
    });
    fixture = TestBed.createComponent(ImsTrnReorderlevelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
