import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnMaterialindentIssueComponent } from './ims-trn-materialindent-issue.component';

describe('ImsTrnMaterialindentIssueComponent', () => {
  let component: ImsTrnMaterialindentIssueComponent;
  let fixture: ComponentFixture<ImsTrnMaterialindentIssueComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnMaterialindentIssueComponent]
    });
    fixture = TestBed.createComponent(ImsTrnMaterialindentIssueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
