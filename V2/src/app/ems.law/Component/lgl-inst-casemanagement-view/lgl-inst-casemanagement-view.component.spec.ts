import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LglInstCasemanagementViewComponent } from './lgl-inst-casemanagement-view.component';

describe('LglInstCasemanagementViewComponent', () => {
  let component: LglInstCasemanagementViewComponent;
  let fixture: ComponentFixture<LglInstCasemanagementViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LglInstCasemanagementViewComponent]
    });
    fixture = TestBed.createComponent(LglInstCasemanagementViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
