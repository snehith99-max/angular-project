import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstDaytrackereditComponent } from './otl-mst-daytrackeredit.component';

describe('OtlMstDaytrackereditComponent', () => {
  let component: OtlMstDaytrackereditComponent;
  let fixture: ComponentFixture<OtlMstDaytrackereditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstDaytrackereditComponent]
    });
    fixture = TestBed.createComponent(OtlMstDaytrackereditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
