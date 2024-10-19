import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstDaytrackersummaryComponent } from './otl-mst-daytrackersummary.component';

describe('OtlMstDaytrackersummaryComponent', () => {
  let component: OtlMstDaytrackersummaryComponent;
  let fixture: ComponentFixture<OtlMstDaytrackersummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstDaytrackersummaryComponent]
    });
    fixture = TestBed.createComponent(OtlMstDaytrackersummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
