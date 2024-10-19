import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsMstAssignproductComponent } from './ims-mst-assignproduct.component';

describe('ImsMstAssignproductComponent', () => {
  let component: ImsMstAssignproductComponent;
  let fixture: ComponentFixture<ImsMstAssignproductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsMstAssignproductComponent]
    });
    fixture = TestBed.createComponent(ImsMstAssignproductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
