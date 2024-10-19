import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTmycallslogsComponent } from './crm-trn-tmycallslogs.component';

describe('CrmTrnTmycallslogsComponent', () => {
  let component: CrmTrnTmycallslogsComponent;
  let fixture: ComponentFixture<CrmTrnTmycallslogsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTmycallslogsComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTmycallslogsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
