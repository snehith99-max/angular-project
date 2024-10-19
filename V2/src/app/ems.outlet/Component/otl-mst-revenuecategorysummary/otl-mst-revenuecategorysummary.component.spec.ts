import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstRevenuecategorysummaryComponent } from './otl-mst-revenuecategorysummary.component';

describe('OtlMstRevenuecategorysummaryComponent', () => {
  let component: OtlMstRevenuecategorysummaryComponent;
  let fixture: ComponentFixture<OtlMstRevenuecategorysummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstRevenuecategorysummaryComponent]
    });
    fixture = TestBed.createComponent(OtlMstRevenuecategorysummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
