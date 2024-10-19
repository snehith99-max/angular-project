import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LglMstCasemanagementUploadComponent } from './lgl-mst-casemanagement-upload.component';

describe('LglMstCasemanagementUploadComponent', () => {
  let component: LglMstCasemanagementUploadComponent;
  let fixture: ComponentFixture<LglMstCasemanagementUploadComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LglMstCasemanagementUploadComponent]
    });
    fixture = TestBed.createComponent(LglMstCasemanagementUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
