import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LglInstCasemanagementUploadComponent } from './lgl-inst-casemanagement-upload.component';

describe('LglInstCasemanagementUploadComponent', () => {
  let component: LglInstCasemanagementUploadComponent;
  let fixture: ComponentFixture<LglInstCasemanagementUploadComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LglInstCasemanagementUploadComponent]
    });
    fixture = TestBed.createComponent(LglInstCasemanagementUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
