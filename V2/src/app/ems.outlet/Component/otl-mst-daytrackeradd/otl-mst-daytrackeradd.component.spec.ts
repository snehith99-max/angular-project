import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstDaytrackeraddComponent } from './otl-mst-daytrackeradd.component';

describe('OtlMstDaytrackeraddComponent', () => {
  let component: OtlMstDaytrackeraddComponent;
  let fixture: ComponentFixture<OtlMstDaytrackeraddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstDaytrackeraddComponent]
    });
    fixture = TestBed.createComponent(OtlMstDaytrackeraddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
