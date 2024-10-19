import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstDaytrackerviewComponent } from './otl-mst-daytrackerview.component';

describe('OtlMstDaytrackerviewComponent', () => {
  let component: OtlMstDaytrackerviewComponent;
  let fixture: ComponentFixture<OtlMstDaytrackerviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstDaytrackerviewComponent]
    });
    fixture = TestBed.createComponent(OtlMstDaytrackerviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
