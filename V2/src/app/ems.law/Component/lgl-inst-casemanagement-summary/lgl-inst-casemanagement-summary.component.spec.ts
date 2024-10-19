import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LglInstCasemanagementSummaryComponent } from './lgl-inst-casemanagement-summary.component';

describe('LglInstCasemanagementSummaryComponent', () => {
  let component: LglInstCasemanagementSummaryComponent;
  let fixture: ComponentFixture<LglInstCasemanagementSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LglInstCasemanagementSummaryComponent]
    });
    fixture = TestBed.createComponent(LglInstCasemanagementSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
